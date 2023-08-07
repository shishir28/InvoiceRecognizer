import dotenv from "dotenv";
import server from "./server";
dotenv.config({ path: ".env" });
const port = process.env.PORT;
server().listen(port);
