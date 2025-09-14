import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTeamById, updateTeam, getTeamWithRiders, deleteTeam } from '../api/teams';
import { getRiders, transferRider } from '../api/riders';
import RemoveRiderButton from '../components/Team/RemoveRiderButton';

export default function TeamDetailPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [team, setTeam] = useState(null);
    const [edit, setEdit] = useState(false);
    const [form, setForm] = useState({});
    const [riders, setRiders] = useState([]);
    const [allRiders, setAllRiders] = useState([]);
    const [addRider, setAddRider] = useState({ riderId: '', seasonId: '', joinDate: '' });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const handleDeleteTeam = async () => {
        if (!window.confirm('Are you sure you want to delete this team?')) return;
        setLoading(true);
        const res = await deleteTeam(id, localStorage.getItem('accessToken'));
        setLoading(false);
        if (res.status === 200) {
            navigate('/teams');
        } else {
            setError(res.error?.message || 'Delete team failed');
        }
    };

    useEffect(() => {
        getTeamById(id, localStorage.getItem('accessToken')).then(res => setTeam(res.data));
        getTeamWithRiders(id, localStorage.getItem('accessToken')).then(res => setRiders(res.data?.riders || []));
        getRiders({ isActive: true }, localStorage.getItem('accessToken')).then(res => setAllRiders(res.data?.items || []));
    }, [id]);

    const handleEdit = () => {
        setForm({
            name: team.name,
            shortName: team.shortName,
            countryCode: team.countryCode,
            countryName: team.countryName,
            countryFlag: team.countryFlag,
            foundedYear: team.foundedYear?.slice(0, 10) || '',
            description: team.description
        });
        setEdit(true);
    };

    const handleSave = async () => {
        setLoading(true);
        const res = await updateTeam(id, form, localStorage.getItem('accessToken'));
        setLoading(false);
        if (res.status === 200) {
            setEdit(false);
            setTeam(t => ({ ...t, ...form }));
        } else {
            setError(res.error?.message || 'Update failed');
        }
    };

    const handleAddRider = async () => {
        setLoading(true);
        const res = await transferRider(addRider.riderId, { teamId: id, seasonId: addRider.seasonId, joinDate: addRider.joinDate }, localStorage.getItem('accessToken'));
        setLoading(false);
        if (res.status === 200) {
            setRiders(r => [...r, allRiders.find(rid => rid.id === addRider.riderId)]);
        } else {
            setError(res.error?.message || 'Add rider failed');
        }
    };

    if (!team) return <div>Loading...</div>;

    return (
        <div style={{ maxWidth: 700, margin: '40px auto', padding: 24, border: '1px solid #ccc', borderRadius: 8 }}>
            <h2>Team Detail</h2>
            {error && <div style={{ color: 'red' }}>{error}</div>}
            {edit ? (
                <>
                    <input name="name" value={form.name} onChange={e => setForm(f => ({ ...f, name: e.target.value }))} placeholder="Team Name" />
                    <input name="shortName" value={form.shortName} onChange={e => setForm(f => ({ ...f, shortName: e.target.value }))} placeholder="Short Name" />
                    <input name="countryCode" value={form.countryCode} onChange={e => setForm(f => ({ ...f, countryCode: e.target.value }))} placeholder="Country Code" />
                    <input name="countryName" value={form.countryName} onChange={e => setForm(f => ({ ...f, countryName: e.target.value }))} placeholder="Country Name" />
                    <input name="countryFlag" value={form.countryFlag} onChange={e => setForm(f => ({ ...f, countryFlag: e.target.value }))} placeholder="Country Flag" />
                    <input name="foundedYear" value={form.foundedYear} onChange={e => setForm(f => ({ ...f, foundedYear: e.target.value }))} placeholder="Founded Year" type="date" />
                    <input name="description" value={form.description} onChange={e => setForm(f => ({ ...f, description: e.target.value }))} placeholder="Description" />
                    <button onClick={handleSave} disabled={loading}>{loading ? 'Saving...' : 'Save'}</button>
                    <button onClick={() => setEdit(false)}>Cancel</button>
                </>
            ) : (
                <>
                    <div><b>Name:</b> {team.name}</div>
                    <div><b>Short Name:</b> {team.shortName}</div>
                    <div><b>Country:</b> {team.countryFlag} {team.countryName}</div>
                    <div><b>Founded:</b> {team.foundedYear?.slice(0, 10)}</div>
                    <div><b>Description:</b> {team.description}</div>
                    <button onClick={handleEdit}>Edit Info</button>
                </>
            )}
            <hr />
            <h3>Current Riders</h3>
            <ul>
                {riders.map(r => (
                    <li key={r.id}>
                        {r.fullName} #{r.racingNumber}
                        <RemoveRiderButton riderId={r.id} onRemoved={async () => {
                            if (window.confirm('Are you sure you want to remove this rider from the team?')) {
                                // Sau khi remove, reload lại danh sách riders từ API
                                const res = await getTeamWithRiders(id, localStorage.getItem('accessToken'));
                                setRiders(res.data?.riders || []);
                            }
                        }} />
                    </li>
                ))}
            </ul>
            <h4>Add Rider to Team</h4>
            <select value={addRider.riderId} onChange={e => setAddRider(a => ({ ...a, riderId: e.target.value }))}>
                <option value="">Select Rider</option>
                {allRiders.filter(r => !r.currentTeamId).map(r => <option key={r.id} value={r.id}>{r.fullName} #{r.racingNumber}</option>)}
            </select>
            <input type="text" placeholder="Season ID" value={addRider.seasonId} onChange={e => setAddRider(a => ({ ...a, seasonId: e.target.value }))} />
            <input type="date" value={addRider.joinDate} onChange={e => setAddRider(a => ({ ...a, joinDate: e.target.value }))} />
            <button onClick={handleAddRider} disabled={loading}>Add Rider</button>
            <hr />
            <button onClick={() => {
                if (window.confirm('Bạn có chắc chắn muốn xóa team này không?')) handleDeleteTeam();
            }} style={{ color: 'red', marginLeft: 8 }} disabled={loading}>Delete Team</button>
        </div>
    );
}
