import axios from "../../utils/axiosCustomize";

const resource = "listings"

const normalizePaging = (data, fallback = {}) => ({
    items: data?.items ?? data?.Items ?? [],
    totalCount: data?.totalCount ?? data?.TotalCount ?? 0,
    pageNumber: data?.pageNumber ?? data?.PageNumber ?? fallback.pageNumber ?? 1,
    pageSize: data?.pageSize ?? data?.PageSize ?? fallback.pageSize ?? 20
});

const create = (body) => axios.post(`${resource}`, body)
const update = (body) => axios.put(`${resource}`, body)
const getById = (id) =>
    axios
        .get(`${resource}/${id}`)
        .then((response) => response?.data ?? null)
const remove = (id) => axios.delete(`${resource}/${id}`)
const listByStatus = (status, params = {}) => {
    // status values: active, drafts, scheduled, ended
    const url = status === 'active' ? `${resource}/active` : `${resource}/${status}`;
    const fallback = {
        pageNumber: params?.pageNumber ?? params?.PageNumber ?? 1,
        pageSize: params?.pageSize ?? params?.PageSize ?? 20
    };
    const queryParams = Object.fromEntries(
        Object.entries(params).filter(([, value]) => value !== undefined && value !== null)
    );
    return axios.get(url, { params: queryParams }).then((r) => normalizePaging(r?.data, fallback));
}

const listUnsoldNotRelisted = (params = {}) => {
    // backend exposes ended endpoint that accepts SoldStatus and RelistStatus
    const query = {
        searchTerm: params?.searchTerm ?? params?.SearchTerm,
        pageNumber: params?.pageNumber ?? params?.PageNumber,
        pageSize: params?.pageSize ?? params?.PageSize,
        fromDate: params?.fromDate ?? params?.FromDate,
        soldStatus: params?.soldStatus ?? params?.SoldStatus ?? 1,
        relistStatus: params?.relistStatus ?? params?.RelistStatus ?? 1
    };
    const queryParams = Object.fromEntries(
        Object.entries(query).filter(([, value]) => value !== undefined && value !== null)
    );
    const fallback = {
        pageNumber: query.pageNumber ?? 1,
        pageSize: query.pageSize ?? 20
    };
    return axios.get(`${resource}/ended`, { params: queryParams }).then((r) => normalizePaging(r?.data, fallback));
}

const bulkDeleteDrafts = (listingIds = []) =>
    axios
        .post(`${resource}/drafts/bulk-delete`, { listingIds })
        .then((response) => response?.data ?? null);

const bulkCopyDrafts = (listingIds = []) =>
    axios
        .post(`${resource}/drafts/copy`, { listingIds })
        .then((response) => response?.data ?? null);

const relist = (listingIds = [], mode = 0) =>
    axios
        .post(`${resource}/relist`, { listingIds, mode })
        .then((response) => response?.data ?? null);

const sellSimilar = (listingIds = []) =>
    axios
        .post(`${resource}/sell-similar`, { listingIds })
        .then((response) => response?.data ?? null);

const endListings = (listingIds = []) =>
    axios
        .post(`${resource}/end`, { listingIds })
        .then((response) => response?.data ?? null);

const sendToDrafts = (listingIds = []) =>
    axios
        .post(`${resource}/send-to-drafts`, { listingIds })
        .then((response) => response?.data ?? null);

const sendOffers = (listingIds = []) =>
    axios
        .post(`${resource}/send-offers`, { listingIds })
        .then((response) => response?.data ?? null);

const promoteListings = (listingIds = []) =>
    axios
        .post(`${resource}/promote`, { listingIds })
        .then((response) => response?.data ?? null);

const archiveListings = (listingIds = []) =>
    axios
        .post(`${resource}/archive`, { listingIds })
        .then((response) => response?.data ?? null);

const exportCsv = (params = {}) =>
    axios
        .get(`${resource}/export`, {
            params,
            paramsSerializer: {
                indexes: null
            },
            responseType: "blob"
        })
        .then((response) => response?.data ?? null);

const importCsv = (file) => {
    const formData = new FormData();
    formData.append("file", file);

    return axios
        .post(`${resource}/import`, formData, {
            headers: { "Content-Type": "multipart/form-data" }
        })
        .then((response) => response?.data ?? null);
};

const ListingService = {
    create,
    update,
    getById,
    remove,
    listByStatus,
    listUnsoldNotRelisted,
    bulkDeleteDrafts,
    bulkCopyDrafts,
    relist,
    sellSimilar,
    endListings,
    sendToDrafts,
    sendOffers,
    promoteListings,
    archiveListings,
    exportCsv,
    importCsv
}
export default ListingService