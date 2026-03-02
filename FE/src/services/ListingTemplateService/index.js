import axios from "../../utils/axiosCustomize";

const resource = "listing-templates";

const normalizePaging = (data, fallback = {}) => ({
    items: data?.items ?? data?.Items ?? [],
    totalCount: data?.totalCount ?? data?.TotalCount ?? 0,
    pageNumber: data?.pageNumber ?? data?.PageNumber ?? fallback.pageNumber ?? 1,
    pageSize: data?.pageSize ?? data?.PageSize ?? fallback.pageSize ?? 20
});

const list = (params = {}) => {
    const fallback = {
        pageNumber: params?.pageNumber ?? params?.PageNumber ?? 1,
        pageSize: params?.pageSize ?? params?.PageSize ?? 20
    };

    const queryParams = Object.fromEntries(
        Object.entries(params).filter(([, value]) => value !== undefined && value !== null)
    );

    return axios
        .get(resource, { params: queryParams })
        .then((response) => normalizePaging(response?.data, fallback));
};

const getById = (id) =>
    axios
        .get(`${resource}/${id}`)
        .then((response) => response?.data ?? null);

const create = (body) => axios.post(resource, body);

const update = (id, body) => axios.put(`${resource}/${id}`, body);

const remove = (id) => axios.delete(`${resource}/${id}`);

const clone = (id, body = {}) => axios.post(`${resource}/${id}/clone`, body);

const ListingTemplateService = {
    list,
    getById,
    create,
    update,
    remove,
    clone
};

export default ListingTemplateService;
