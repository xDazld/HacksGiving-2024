from connection_manager import ConnectionManager
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
from gpt4all import GPT4All
from user import User


app = FastAPI()

model = GPT4All("Llama-3.2-1B-Instruct-Q4_0.gguf")

manager = ConnectionManager()

users: dict[int, User] = dict()


def prompt_model(user: User, prompt: str):
    with model.chat_session():
        if len(user.get_history()) != 0:
            model._history = user.get_history()
        else:
            user.set_history(model._history)
        return model.generate(f"{prompt}", streaming=True)


@app.websocket("/ws/{client_id}")
async def websocket_endpoint(websocket: WebSocket, client_id: int):
    await manager.connect(websocket)
    user = User(client_id)
    users[client_id] = user
    print(f"Client {client_id} connected")
    try:
        while True:
            data = await websocket.receive_json()
            event = data["event"]

            if event == "newChat":
                prompt = user.new_chat(data)
                gen = prompt_model(user, prompt)
                for message in gen:
                    message_dict = {
                        "message": message,
                    }
                    await manager.emit(message_dict, websocket)
            elif event == "chatMessage":
                prompt = user.chat_message(data)
                gen = prompt_model(user, prompt)
                for message in gen:
                    message_dict = {
                        "message": message,
                    }
                    await manager.emit(message_dict, websocket)

    except WebSocketDisconnect:
        manager.disconnect(websocket)
        print(f"Client {client_id} disconnected")
