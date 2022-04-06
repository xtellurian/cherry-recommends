import { executeFetch } from "./client/apiClient";
import * as pr from "./commonEntity/propertiesApiUtil";
export const fetchPromotionsAsync = async ({ token, page, searchTerm, promotionType, benefitType, weeksAgo, }) => {
    return await executeFetch({
        path: "api/Promotions",
        token,
        page,
        query: {
            "q.term": searchTerm,
            "q.weeksAgo": weeksAgo,
            promotionType: promotionType,
            benefitType: benefitType,
        },
    });
};
export const fetchPromotionAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Promotions/${id}`,
        token,
    });
};
export const createPromotionAsync = async ({ token, promotion, }) => {
    return await executeFetch({
        path: "api/Promotions",
        token,
        method: "post",
        body: promotion,
    });
};
export const updatePromotionAsync = async ({ token, id, promotion, }) => {
    return await executeFetch({
        path: `api/Promotions/${id}`,
        token,
        method: "post",
        body: promotion,
    });
};
export const deletePromotionAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Promotions/${id}`,
        token,
        method: "delete",
    });
};
export const getPropertiesAsync = async ({ token, id }) => {
    return await pr.getPropertiesAsync({
        token,
        id,
        api: "Promotions",
    });
};
export const setPropertiesAsync = async ({ token, id, properties, }) => {
    return await pr.setPropertiesAsync({
        token,
        id,
        properties,
        api: "Promotions",
    });
};
