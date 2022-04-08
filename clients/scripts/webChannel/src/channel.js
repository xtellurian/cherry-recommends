import { setBaseUrl, setTenant, apiKeys, channels } from "cherry.ai"

const srcUrl = new URL(document.getElementById("cherry-channel").src)
const params = new URLSearchParams(srcUrl.search);

const baseUrl = params.get("baseUrl")
const tenant = params.get("tenant")
const apiKey = params.get("apiKey");
const channelId = params.get("channelId")

setBaseUrl(baseUrl);
setTenant(tenant)

apiKeys.exchangeApiKeyAsync({ apiKey })
    .then(({ access_token }) => {
        channels.fetchChannelAsync({ id: channelId, token: access_token })
    })