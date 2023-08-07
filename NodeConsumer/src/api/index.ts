import { Router } from "express";
import {
	AzureKeyCredential,
	DocumentAnalysisClient,
} from "@azure/ai-form-recognizer";
import dotenv from "dotenv";

dotenv.config({ path: ".env" });
const endpoint = process.env["FORM_RECOGNIZER_ENDPOINT"];
const apiKey = process.env["FORM_RECOGNIZER_KEY"];
const modelId = process.env["FORM_RECOGNIZER_MODEL_ID"];
const documentURL = process.env["INPUT_DOCUMENT_URL"];
const router: Router = Router();

router.get("/data", async (_req, res) => {
	let r = await getData();
	res.json({
		message: "All good!",
	});
});

const getData = async function (): Promise<void> {
	const credential = new AzureKeyCredential(apiKey!);
	const client = new DocumentAnalysisClient(endpoint!, credential);
	const poller = await client.beginAnalyzeDocumentFromUrl(
		modelId!,
		documentURL!
	);
	const { documents } = await poller.pollUntilDone();
	for (const document of documents || []) {
		console.log(
			`Document of ${
				document!.docType
			} type and  ${modelId} model has confidence ${document!.confidence}\n\n`
		);

		for (const [name, field] of Object.entries(document.fields)) {
			// @ts-ignore
			let value = field.value;
			console.log(`${name} : '${value}'`);
		}
	}
	console.log("..........................................");
};
export default router;
