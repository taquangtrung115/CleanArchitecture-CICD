import axios from 'axios';

const BASE_URL = 'https://localhost:5258/api/v1/motogp/teams';

export const getTeams = (params, token) =>
    axios.get(BASE_URL, {
        params,
        headers: { Authorization: `Bearer ${token}` }
    });

export const getTeamById = (id, token) =>
    axios.get(`${BASE_URL}/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const getTeamWithRiders = (id, token) =>
    axios.get(`${BASE_URL}/${id}/with-riders`, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const createTeam = (data, token) =>
    axios.post(BASE_URL, data, {
        headers: { Authorization: `Bearer ${token}` }
    });

export const updateTeam = (id, data, token) =>
    axios.put(`${BASE_URL}/${id}`, data, {
        headers: { Authorization: `Bearer ${token}` }
    });
