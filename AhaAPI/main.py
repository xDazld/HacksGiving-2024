from fastapi import FastAPI
from gpt4all import GPT4All
app = FastAPI()
model = GPT4All("Llama-3.2-1B-Instruct-Q4_0.gguf")

@app.get("/")
async def root():
    with model.chat_session():
        return {"message": model.generate("Hello World")}



@app.get("/hello/{name}")
async def say_hello(name: str):
    with model.chat_session():
        return {"message": model.generate(f"Hello {name}")}
