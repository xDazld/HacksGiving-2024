from gpt4all import GPT4All

from knowledge_loading import ExhibitEmbeds
from user import User

embeds = ExhibitEmbeds()


def prompt_model(user: User, prompt: str):
    """
    Generate a response to a user prompt
    :param user: the user that made the prompt
    :param prompt: the prompt given by the user
    :return: Generator of tokens for response
    """
    model = GPT4All("Llama-3.2-1B-Instruct-Q4_0.gguf", n_ctx=4096)
    with model.chat_session(
        system_prompt="You are an AI-driven assistant that enhances visitor "
        "interactions with exhibits. Allow users to scale "
        "content difficulty and personalize their experience "
        "using age, language, interest, and topic. Draw "
        "connections between different content. Find additional "
        "fun-facts/information based on topics the user "
        "expresses interest in.\nWith each prompt, you will be "
        "given metadata about the exhibit a user is at along "
        "with a prompt from the user. You should inspire 'Aha!' moments. "
        "Respond with empathy. The user is "
        + str(user.get_age())
        + " years old and speaks "
        + user.user_data["language"]
        + "."
    ):
        if len(user.get_history()) != 0:
            model._history = user.get_history()
        else:
            user.set_history(model._history)
        return model.generate(
            f"Current Exhibit Context:\n{embeds.get_embedding(user.get_exhibit())}"
            f"\nUser Prompt:\n{prompt}",
            streaming=True,
            max_tokens=2048,
        )
