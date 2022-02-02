import { randomBytes } from "crypto";

export const randomString = (length: number): string => {
    return randomBytes(64).toString("hex").substring(0, 12)
}