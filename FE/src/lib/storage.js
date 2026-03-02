const STORAGE = {
    TOKEN: "token",
    REFRESH_TOKEN: "refresh-token",
    USER_INFO: "user-info",
    REMEMBER_LOGIN: "remember-login",
    KEY_MENU_ACTIVE: "key-active",
    TABS_PAGE_ACTIVE: "tabs-page-active",
    ERROR_EXTENSION: "error-extension",
    DEV_MODE: "dev-mode",
}

export const getStorage = name => {
    const remember = localStorage.getItem(STORAGE.REMEMBER_LOGIN)
    let data
    if (remember) {
        data =
            typeof window !== "undefined" && name !== undefined
                ? localStorage.getItem(name)
                : ""
    } else {
        data =
            typeof window !== "undefined" && name !== undefined
                ? sessionStorage.getItem(name)
                : ""
    }
    try {
        if (data) return JSON.parse(data)
    } catch (err) {
        return data
    }
}

export const setStorage = (name, value) => {
    const remember = localStorage.getItem(STORAGE.REMEMBER_LOGIN)
    // const stringify = typeof value !== "string" ? JSON.stringify(value) : value
    const data = value
    if (remember) {
        return localStorage.setItem(name, data)
    } else {
        return sessionStorage.setItem(name, data)
    }
}

export const deleteStorage = name => {
    const remember = localStorage.getItem(STORAGE.REMEMBER_LOGIN)
    if (remember) {
        return localStorage.removeItem(name)
    } else {
        return sessionStorage.removeItem(name)
    }
}

export const clearStorage = () => {
    const remember = localStorage.getItem(STORAGE.REMEMBER_LOGIN)
    if (remember) {
        return localStorage.clear()
    } else {
        return sessionStorage.clear()
    }
}

export default STORAGE
