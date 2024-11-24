from fastapi import FastAPI, WebSocket, WebSocketDisconnect

from chat import prompt_model
from connection_manager import ConnectionManager
from knowledge_loading import ExhibitEmbeds
from user import User

app = FastAPI()

manager = ConnectionManager()

users: dict[int, User] = {}

embeds = ExhibitEmbeds()


@app.websocket("/ws/{client_id}")
async def websocket_endpoint(websocket: WebSocket, client_id: int) -> None:
    """
    Handles websocket connections
    :param websocket: the websocket connection
    :param client_id: the client id
    """
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
                    await manager.emit_text(message_dict, websocket)
                    print(message, end="")
                await manager.emit(
                    {
                        "message": "END CHAT",
                    },
                    websocket,
                )
                print()
            elif event == "chatMessage":
                print(f"{client_id} sent chat message")
                prompt = user.chat_message(data)
                print(prompt)
                gen = prompt_model(user, prompt)
                for message in gen:
                    message_dict = {
                        "message": message,
                    }
                    await manager.emit_text(message_dict, websocket)
                    print(message, end="")
                await manager.emit(
                    {
                        "message": "END CHAT",
                    },
                    websocket,
                )
                print()

    except WebSocketDisconnect:
        manager.disconnect(websocket)
        print(f"Client {client_id} disconnected")
    finally:
        user.save()


@app.get("/exhibits/topics")
async def get_exhibit_topics() -> None:
    """
    Get exhibit topics
    """
    return embeds.get_topics()

@app.get("/")
async def root():
    return "AhaAPI is running"