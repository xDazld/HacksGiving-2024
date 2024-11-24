import json
import os

from gpt4all.gpt4all import MessageType

USER_DB_PATH = ".config/hacksgiving-2024/users/"

os.makedirs(USER_DB_PATH, exist_ok=True)


class User:
    """
    User class to store user data
    """

    def __init__(self, id: int):
        """
        Initializes the User class
        :param id: the user id
        """
        if os.path.exists(f"{USER_DB_PATH}{id}.json"):
            with open(f"{USER_DB_PATH}{id}.json", "r") as json_file:
                self.user_data = json.load(json_file)
        else:
            self.user_data = {
                "id": id,
                "age": 10,
                "language": "en",
                "interests": [],
                "history": [],
            }
        self.save()

    def save(self) -> None:
        """
        Saves the user data
        :return:
        """
        with open(f"{USER_DB_PATH}{self.user_data['id']}.json", "w") as json_file:
            json.dump(self.user_data, json_file)

    def __del__(self):
        """
        Saves the user data when the object is deleted
        :return:
        """
        self.save()

    def get_history(self) -> list[MessageType]:
        """
        Gets the user history
        :return: the user history
        """
        return self.user_data["history"]

    def set_history(self, history: list[MessageType]) -> None:
        """
        Sets the user history
        :param history: the user history
        :return:
        """
        self.user_data["history"] = history

    def new_chat(self, data: dict) -> str:
        """
        Starts a new chat
        :param data: the data for the chat
        :return: the initial prompt
        """
        context = data["userContext"]
        self.user_data["age"] = context["age"]
        self.user_data["language"] = context["language"]
        self.user_data["interests"] = context["interests"]
        self.user_data["currentExhibit"] = data["currentExhibit"]
        return "Tell me about this exhibit."

    def chat_message(self, data: dict) -> str:
        """
        Processes a chat message
        :param data: the chat message data
        :return: the prompt
        """
        self.user_data["currentExhibit"] = data["currentExhibit"]
        return data["prompt"]

    def get_age(self) -> int:
        """
        Gets the user age
        :return: the user
        """
        return self.user_data["age"]

    def get_language(self) -> str:
        """
        Gets the user language
        :return: the user language
        """
        return self.user_data["language"]

    def get_exhibit(self) -> str:
        """
        Gets the current exhibit
        :return: the current exhibit
        """
        return self.user_data["currentExhibit"]
