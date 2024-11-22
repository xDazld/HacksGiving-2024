from fastapi import FastAPI
from fastapi.responses import StreamingResponse
from gpt4all import GPT4All

app = FastAPI()
model = GPT4All("Llama-3.2-1B-Instruct-Q4_0.gguf")
histories = dict()

@app.get("/")
async def root():
    with model.chat_session():
        return StreamingResponse(model.generate("Hello World", streaming=True))


@app.get("/chat/{name}/{prompt}")
async def prompt_model(name: str, prompt: str):
    with model.chat_session():
        if name in histories:
            model._history = histories[name]
        else:
            histories[name] = model._history
        return StreamingResponse(model.generate(f"{prompt}", streaming=True))


@app.get("/history/{user_id}")
async def get_history(user_id: str):
    return histories[user_id]
