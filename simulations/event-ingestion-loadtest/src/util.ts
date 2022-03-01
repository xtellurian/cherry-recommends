import { randomBytes } from "crypto";

export const randomString = (length: number): string => {
  return randomBytes(64).toString("hex").substring(0, 12);
};

export const getDaysArray = function (start: Date, end: Date): Date[] {
  for (
    var arr = [], dt = new Date(start);
    dt <= end;
    dt.setDate(dt.getDate() + 1)
  ) {
    arr.push(new Date(dt));
  }
  return arr;
};
