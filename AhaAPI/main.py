from fastapi import FastAPI
from fastapi.responses import StreamingResponse
from gpt4all import GPT4All

from user import User

app = FastAPI()
model = GPT4All("Llama-3.2-1B-Instruct-Q4_0.gguf")

user_cache: dict[str, User] = dict()

@app.get("/")
async def root():
    with model.chat_session():
        return StreamingResponse(model.generate("Hello World", streaming=True))


@app.get("/chat/{name}/{prompt}")
async def prompt_model(name: str, prompt: str):
    with model.chat_session():
        if len(_load_user(name).get_history()) != 0:
            model._history = _load_user(name).get_history()
        else:
            _load_user(name).set_history(model._history)
        return StreamingResponse(model.generate(f"{prompt}", streaming=True))


@app.get("/history/{user_id}")
async def get_history(user_id: str):
    return _load_user(user_id).get_history()


@app.get("/user/profile/{user_id}")
async def get_user_profile(user_id: str):
    return f"User: {user_id} Profile"


def _load_user(username: str) -> User:
    if username in user_cache:
        return user_cache[username]
    user = User(username)
    user_cache[username] = user
    return user


@app.get("/user/profile/{user_id}/create")
async def get_user_profile(user_id: str):
    return User.create_account(user_id, "20", "en")
