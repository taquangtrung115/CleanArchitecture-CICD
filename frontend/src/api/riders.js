import axios from 'axios';

const BASE_URL = 'https://localhost:5258/api/carter/v1/motogp/riders';

export const getRiders = (params, token) =>
    axios.get(BASE_URL, {
        params,
        headers: { Authorization: `Bearer ${token}` }
    });

export const getRiderById = (id, token) =>
    axios.get(`${BASE_URL}/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const getRiderByNumber = (number, token) =>
    axios.get(`${BASE_URL}/racing-number/${number}`, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const createRider = (data, token) =>
    axios.post(BASE_URL, data, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const updateRiderPersonalInfo = (id, data, token) =>
    axios.put(`${BASE_URL}/${id}/personal-info`, data, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const transferRider = (id, data, token) =>
    axios.put(`${BASE_URL}/${id}/transfer`, data, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const retireRider = (id, data, token) =>
    axios.put(`${BASE_URL}/${id}/retire`, data, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const comebackRider = (id, token) =>
    axios.put(`${BASE_URL}/${id}/comeback`, {}, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const deleteRider = (id, token) =>
    axios.delete(`${BASE_URL}/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
    });
