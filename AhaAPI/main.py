from fastapi import FastAPI, WebSocket, WebSocketDisconnect
from fastapi.middleware.cors import CORSMiddleware

from chat import prompt_model
from connection_manager import ConnectionManager
from knowledge_loading import ExhibitEmbeds
from user import User

app = FastAPI()
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

manager = ConnectionManager()

users: dict[int, User] = {}

embeds = ExhibitEmbeds()


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
                print(f"{client_id} made new chat")
                prompt = user.new_chat(data)
                gen = prompt_model(user, prompt)
                for message in gen:
                    message_dict = {
                        "message": message,
                    }
                    await manager.emit(message_dict, websocket)
                    print(message, end="")
                await manager.emit(
                    {
                        "message": "\x04",
                    },
                    websocket,
                )
            elif event == "chatMessage":
                print(f"{client_id} sent chat message")
                prompt = user.chat_message(data)
                gen = prompt_model(user, prompt)
                for message in gen:
                    message_dict = {
                        "message": message,
                    }
                    await manager.emit(message_dict, websocket)
                    print(message, end="")
                await manager.emit(
                    {
                        "message": "\x04",
                    },
                    websocket,
                )

    except WebSocketDisconnect:
        manager.disconnect(websocket)
        print(f"Client {client_id} disconnected")


@app.get("/exhibits/topics")
async def get_exhibit_topics():
    return embeds.get_topics()
