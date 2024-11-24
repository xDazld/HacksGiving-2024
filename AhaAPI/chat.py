from gpt4all import GPT4All

from knowledge_loading import ExhibitEmbeds
from user import User

embeds = ExhibitEmbeds()
model = GPT4All("Llama-3.2-1B-Instruct-Q4_0.gguf")


def prompt_model(user: User, prompt: str, exhibit: str):
    with model.chat_session(system_prompt="You are an AI-driven assistant that enhances visitor "
                                          "interactions with exhibits. Allow users to scale "
                                          "content difficulty and personalize their experience "
                                          "using age, language, interest, and topic. Draw "
                                          "connections between different content. Find additional "
                                          "fun-facts/information based on topics the user "
                                          "expresses interest in.\nWith each prompt, you will be "
                                          "given information about the exhibit a user is at along "
                                          "with their prompt. You should inspire 'Aha!' moments. "
                                          "Respond with empathy."):
        if len(user.get_history()) != 0:
            model._history = user.get_history()
        else:
            user.set_history(model._history)
        return model.generate(f"Current Exhibit Context:\n{embeds.get_embedding(exhibit)}"
                              f"\nUser Prompt:\n{prompt}", streaming=True, max_tokens=2048)
