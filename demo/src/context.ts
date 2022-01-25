import { existsSync, rmSync } from "fs";
import { promises as fs } from "fs";

interface Context {
  environment?: any;
  recommender?: any;
  items?: any[];
  customer?: any;
}
const fileName = "../context.json";
export const loadContext = async (): Promise<Context> => {
  if (existsSync(fileName)) {
    const f = await fs.readFile(fileName);
    return JSON.parse(f.toString());
  } else {
    throw new Error("No file found");
  }
};

export const saveContext = async (context: Context) => {
  await fs.writeFile(fileName, JSON.stringify(context));
};

export const destroyContext = async (context: Context) => {
  await fs.unlink(fileName);
};

export const newContext = (): Context => {
  return {
    environment: null,
  };
};
