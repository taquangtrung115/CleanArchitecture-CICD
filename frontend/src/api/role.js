// src/api/role.js
import axiosInstance from './axios';

const ROLE_ENDPOINT = '/api/v1/roles';

// 1. Tạo vai trò mới
export const createRole = async ({ name, description, roleCode }) => {
    try {
        const response = await axiosInstance.post(ROLE_ENDPOINT, { name, description, roleCode });
        return { data: response.data, status: response.status, error: null };
    } catch (error) {
        return {
            data: null,
            status: error.response ? error.response.status : 500,
            error: error.response ? error.response.data : error.message
        };
    }
};

// 2. Lấy danh sách vai trò
export const getRoles = async (page = 1, pageSize = 20, searchTerm = '') => {
    try {
        const params = new URLSearchParams({ page, pageSize });
        if (searchTerm) params.append('searchTerm', searchTerm);
        const response = await axiosInstance.get(`${ROLE_ENDPOINT}?${params.toString()}`);
        return { data: response.data, status: response.status, error: null };
    } catch (error) {
        return {
            data: null,
            status: error.response ? error.response.status : 500,
            error: error.response ? error.response.data : error.message
        };
    }
};

// 3. Lấy chi tiết vai trò
export const getRoleDetail = async (roleId) => {
    try {
        const response = await axiosInstance.get(`${ROLE_ENDPOINT}/${roleId}`);
        return { data: response.data, status: response.status, error: null };
    } catch (error) {
        return {
            data: null,
            status: error.response ? error.response.status : 500,
            error: error.response ? error.response.data : error.message
        };
    }
};

// 4. Cập nhật vai trò
export const updateRole = async (roleId, { name, description, roleCode }) => {
    try {
        const response = await axiosInstance.put(`${ROLE_ENDPOINT}/${roleId}`, { roleId, name, description, roleCode });
        return { data: response.data, status: response.status, error: null };
    } catch (error) {
        return {
            data: null,
            status: error.response ? error.response.status : 500,
            error: error.response ? error.response.data : error.message
        };
    }
};

// 5. Xóa vai trò
export const deleteRole = async (roleId) => {
    try {
        const response = await axiosInstance.delete(`${ROLE_ENDPOINT}/${roleId}`);
        return { data: response.data, status: response.status, error: null };
    } catch (error) {
        return {
            data: null,
            status: error.response ? error.response.status : 500,
            error: error.response ? error.response.data : error.message
        };
    }
};

// 6. Lấy danh sách user thuộc vai trò
export const getRoleUsers = async (roleId) => {
    try {
        const response = await axiosInstance.get(`${ROLE_ENDPOINT}/${roleId}/users`);
        return { data: response.data, status: response.status, error: null };
    } catch (error) {
        return {
            data: null,
            status: error.response ? error.response.status : 500,
            error: error.response ? error.response.data : error.message
        };
    }
};

// 7. Lấy danh sách quyền của vai trò
export const getRolePermissions = async (roleId) => {
    try {
        const response = await axiosInstance.get(`${ROLE_ENDPOINT}/${roleId}/permissions`);
        return { data: response.data, status: response.status, error: null };
    } catch (error) {
        return {
            data: null,
            status: error.response ? error.response.status : 500,
            error: error.response ? error.response.data : error.message
        };
    }
};

// 8. Gán quyền cho vai trò
export const addRolePermission = async (roleId, { functionId, actionId }) => {
    try {
        const response = await axiosInstance.post(`${ROLE_ENDPOINT}/${roleId}/permissions`, { roleId, functionId, actionId });
        return { data: response.data, status: response.status, error: null };
    } catch (error) {
        return {
            data: null,
            status: error.response ? error.response.status : 500,
            error: error.response ? error.response.data : error.message
        };
    }
};

// 9. Thu hồi quyền khỏi vai trò
export const removeRolePermission = async (roleId, { functionId, actionId }) => {
    try {
        const response = await axiosInstance.delete(`${ROLE_ENDPOINT}/${roleId}/permissions`, { data: { roleId, functionId, actionId } });
        return { data: response.data, status: response.status, error: null };
    } catch (error) {
        return {
            data: null,
            status: error.response ? error.response.status : 500,
            error: error.response ? error.response.data : error.message
        };
    }
};
