import os
from dotenv import load_dotenv

from azure.core.credentials import AzureKeyCredential
from azure.ai.formrecognizer import DocumentAnalysisClient

import pandas as pd

from tabulate import tabulate

load_dotenv()

endpoint = os.environ['ENV_FORM_RECOGNIZER_ENDPOINT']
key = os.environ['ENV_FORM_RECOGNIZER_KEY']
model_id = os.environ['ENV_FORM_RECOGNIZER_MODEL_ID']
formUrl = os.environ['ENV_INPUT_DOCUMENT_URL']

document_analysis_client = DocumentAnalysisClient(
    endpoint=endpoint, credential=AzureKeyCredential(key)
)

poller = document_analysis_client.begin_analyze_document_from_url(
    model_id, formUrl)
result = poller.result()

for idx, document in enumerate(result.documents):
    print("Document of {} type and {} model has confidence {}".format(
        document.doc_type, result.model_id, document.confidence))

    myDictionary = {'FieldName': 'FieldValue'}
    for name, field in document.fields.items():
        field_value = field.value if field.value else field.content
        myDictionary[name] = field_value

    df = pd.DataFrame(myDictionary.items())

    print(tabulate(df))

print("..........................................")
