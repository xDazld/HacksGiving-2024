class ExhibitEmbeds:
    def __init__(self):
        from pandas import read_excel
        self._embeddings = read_excel("../data/provided/Exhibit_Master_List.xlsx", skiprows=1,
                                      index_col="Experience Name")

    def get_embedding(self, exhibit: str):
        return self._embeddings.loc[exhibit].to_markdown()

    def get_topics(self):
        topics_series = self._embeddings['Topics'].dropna()
        all_topics = set()
        for topics in topics_series:
            all_topics.update(topic.strip() for topic in topics.split(','))
        return list(all_topics)
