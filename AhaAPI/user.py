import json
import os

from gpt4all.gpt4all import MessageType

USER_DB_PATH = ".config/hacksgiving-2024/users/"

if not os.path.exists(USER_DB_PATH):
    os.makedirs(USER_DB_PATH)


class User:

    def __init__(self, username: str):
        self.username = username
        with open(f"{USER_DB_PATH}{self.username}.json", "r") as json_file:
            self.user_data = json.load(json_file)

    @staticmethod
    def create_account(username: str, age: str, language: str) -> bool:
        if not os.path.exists(f"{USER_DB_PATH}{username}.json"):
            with open(f"{USER_DB_PATH}{username}.json", "w") as json_file:
                json.dump(
                    {"username": username, "age": age, "language": language, "history": [dict()]},
                    json_file)
            return True
        return False

    def save(self):
        with open(f"{USER_DB_PATH}{self.username}.json", "w") as json_file:
            json.dump(self.user_data, json_file)

    def __del__(self):
        self.save()

    def get_history(self) -> list[MessageType]:
        return self.user_data["history"]

    def set_history(self, history: list[MessageType]):
        self.user_data["history"] = history
