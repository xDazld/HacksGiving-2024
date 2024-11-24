from fastapi import WebSocket


class ConnectionManager:
    def __init__(self):
        self.clients: list[WebSocket] = []

    async def connect(self, websocket: WebSocket):
        await websocket.accept()
        self.clients.append(websocket)

    def disconnect(self, websocket: WebSocket):
        self.clients.remove(websocket)

    @staticmethod
    async def emit(message: dict, websocket: WebSocket):
        await websocket.send_json(message)

    @staticmethod
    async def emit_text(message: dict, websocket: WebSocket):
        await websocket.send_text(message["message"])

    async def broadcast(self, message: dict):
        for connection in self.clients:
            await connection.send_json(message)
