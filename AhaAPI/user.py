import json
import os

from gpt4all.gpt4all import MessageType

USER_DB_PATH = ".config/hacksgiving-2024/users/"

os.makedirs(USER_DB_PATH, exist_ok=True)


class User:

    def __init__(self, id: int):
        if os.path.exists(f"{USER_DB_PATH}{id}.json"):
            with open(f"{USER_DB_PATH}{id}.json", "r") as json_file:
                self.user_data = json.load(json_file)
        else:
            self.user_data = {
                "id": id,
                "age": None,
                "language": None,
                "interests": [],
                "history": [],
            }

    def save(self):
        with open(f"{USER_DB_PATH}{self.user_data["id"]}.json", "w") as json_file:
            json.dump(self.user_data, json_file)

    def __del__(self):
        self.save()

    def get_history(self) -> list[MessageType]:
        return self.user_data["history"]

    def set_history(self, history: list[MessageType]):
        self.user_data["history"] = history

    def new_chat(self, data: dict) -> str:
        context = data["userContext"]
        self.user_data["age"] = context["age"]
        self.user_data["language"] = context["language"]
        self.user_data["interests"] = context["interests"]
        return "TODO PROMPT"
