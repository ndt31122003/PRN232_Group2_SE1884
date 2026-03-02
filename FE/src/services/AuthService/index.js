import axios from "../../utils/axiosCustomize";
import { apiLogin, apiLogout, apiRefreshToken } from "./urls"

const login = (body) => axios.post(apiLogin, body)
const logout = () => axios.post(apiLogout)
const refreshToken = (refreshToken) =>
    axios.post(
        apiRefreshToken,
        {
            refreshToken,
        },
        {
            skipAuthRefresh: true,
        }
    )
const AuthService = {
    login,
    logout,
    refreshToken,
}
export default AuthService
