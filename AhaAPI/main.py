from fastapi import FastAPI
from fastapi.responses import StreamingResponse
from gpt4all import GPT4All

app = FastAPI()
model = GPT4All("Llama-3.2-1B-Instruct-Q4_0.gguf")


@app.get("/")
async def root():
    with model.chat_session():
        return StreamingResponse(model.generate("Hello World", streaming=True))


@app.get("/hello/{name}")
async def say_hello(name: str):
    with model.chat_session():
        return StreamingResponse(model.generate(f"Hello {name}", streaming=True))
