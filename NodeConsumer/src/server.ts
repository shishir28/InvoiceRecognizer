import express, { Request, Response } from "express";

import http from "http";
import routes from "./api";

const app = () => {
	const app = express();
	app.use(express.json());

	// Routes
	app.use("/", routes);
	app.get("/health", (_, res) => res.status(200).send({ success: false }));

	// All non-specified routes return 404
	app.get("*", (_, res) => res.status(404).send("Not Found"));
	const server = http.createServer(app);
	server.on("listening", () => {
		console.info(
			`Form-Analyzer service listening on port ${process.env.PORT}...\n\n\n`
		);
	});
	return server;
};
export default app;
