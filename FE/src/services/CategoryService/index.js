import axios from "../../utils/axiosCustomize";

const resource = "categories";

const getCategories = (parentId = null) => {
    const params = {};
    if (parentId) {
        params.parentId = parentId;
    }
    return axios
        .get(resource, { params })
        .then((response) => response?.data ?? []);
};

const getCategoryDetail = (categoryId) =>
    axios
        .get(`${resource}/${categoryId}`)
        .then((response) => response?.data ?? null);

const CategoryService = {
    getCategories,
    getCategoryDetail,
};

export default CategoryService;
