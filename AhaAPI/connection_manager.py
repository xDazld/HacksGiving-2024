from fastapi import WebSocket


class ConnectionManager:
    def __init__(self):
        self.clients: list[WebSocket] = []

    async def connect(self, websocket: WebSocket):
        await websocket.accept()
        self.clients.append(websocket)

    def disconnect(self, websocket: WebSocket):
        self.clients.remove(websocket)

    async def emit(self, message: str, websocket: WebSocket):
        await websocket.send_json(message)

    async def broadcast(self, message: str):
        for connection in self.clients:
            await connection.send_json(message)