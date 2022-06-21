#!/usr/bin/env zx

const zone = "cherry.ai";
const rg = "dns";

cd("../../../aws");

const stackOutput = await $`pulumi stack output -j`;
const stack = JSON.parse(stackOutput.stdout);

for (const token of stack.DkimTokens) {
  await $`az network dns record-set cname set-record -g ${rg} -z ${zone} -n ${token}._domainkey -c ${token}.dkim.amazonses.com`;
}
