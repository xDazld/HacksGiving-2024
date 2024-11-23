class ExhibitEmbeds:
    def __init__(self):
        from gpt4all import Embed4All
        from pandas import read_excel

        embedder = Embed4All()
        exhibits = read_excel("../data/provided/Exhibit_Master_List.xlsx", skiprows=1)
        self._embeddings = dict()
        for _, exhibit in exhibits.iterrows():
            self._embeddings[exhibit["Experience Name"]] = embedder.embed(
                exhibit.to_markdown()
            )

    def get_embedding(self, exhibit: str):
        return self._embeddings[exhibit]
