import RemoveRiderButton from '../components/Team/RemoveRiderButton';
import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getRiderById, updateRiderPersonalInfo, transferRider, retireRider, comebackRider, deleteRider } from '../api/riders';
import { getTeams } from '../api/teams';
import TransferButton from '../components/Rider/TransferButton';

export default function RiderDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [rider, setRider] = useState(null);
  const [edit, setEdit] = useState(false);
  const [form, setForm] = useState({});
  const [teams, setTeams] = useState([]);
  const [transfer, setTransfer] = useState({ teamId: '', seasonId: '', joinDate: '' });
  const [retireDate, setRetireDate] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    getRiderById(id, localStorage.getItem('accessToken')).then((res) => setRider(res.data));
    getTeams({}, localStorage.getItem('accessToken')).then((res) => setTeams(res.data?.items || []));
  }, [id]);

  const handleEdit = () => {
    setForm({
      firstName: rider.firstName,
      lastName: rider.lastName,
      nickname: rider.nickname,
      height: rider.height,
      weight: rider.weight
    });
    setEdit(true);
  };

  const handleSave = async () => {
    setLoading(true);
    const res = await updateRiderPersonalInfo(id, form, localStorage.getItem('accessToken'));
    setLoading(false);
    if (res.status === 200) {
      setEdit(false);
      setRider((r) => ({ ...r, ...form }));
    } else {
      setError(res.error?.message || 'Update failed');
    }
  };

  const handleTransfer = async () => {
    setLoading(true);
    const res = await transferRider(id, transfer, localStorage.getItem('accessToken'));
    setLoading(false);
    if (res.status === 200) {
      setRider((r) => ({ ...r, currentTeamId: transfer.teamId }));
    } else {
      setError(res.error?.message || 'Transfer failed');
    }
  };

  const handleRetire = async () => {
    setLoading(true);
    const res = await retireRider(id, { retirementDate: retireDate }, localStorage.getItem('accessToken'));
    setLoading(false);
    if (res.status === 200) {
      setRider((r) => ({ ...r, isActive: false, retirementDate: retireDate }));
    } else {
      setError(res.error?.message || 'Retire failed');
    }
  };

  const handleComeback = async () => {
    setLoading(true);
    const res = await comebackRider(id, localStorage.getItem('accessToken'));
    setLoading(false);
    if (res.status === 200) {
      setRider((r) => ({ ...r, isActive: true }));
    } else {
      setError(res.error?.message || 'Comeback failed');
    }
  };

  const handleDelete = async () => {
    if (!window.confirm('Are you sure you want to delete this rider?')) return;
    setLoading(true);
    const res = await deleteRider(id, localStorage.getItem('accessToken'));
    setLoading(false);
    if (res.status === 200) {
      navigate('/riders');
    } else {
      setError(res.error?.message || 'Delete failed');
    }
  };

  if (!rider) return <div>Loading...</div>;

  return (
    <div style={{ maxWidth: 600, margin: '40px auto', padding: 24, border: '1px solid #ccc', borderRadius: 8 }}>
      <h2>Rider Detail</h2>
      {error && <div style={{ color: 'red' }}>{error}</div>}
      {edit ? (
        <>
          <input
            name="firstName"
            value={form.firstName}
            onChange={(e) => setForm((f) => ({ ...f, firstName: e.target.value }))}
            placeholder="First Name"
          />
          <input
            name="lastName"
            value={form.lastName}
            onChange={(e) => setForm((f) => ({ ...f, lastName: e.target.value }))}
            placeholder="Last Name"
          />
          <input
            name="nickname"
            value={form.nickname}
            onChange={(e) => setForm((f) => ({ ...f, nickname: e.target.value }))}
            placeholder="Nickname"
          />
          <input
            name="height"
            value={form.height}
            onChange={(e) => setForm((f) => ({ ...f, height: e.target.value }))}
            placeholder="Height"
            type="number"
          />
          <input
            name="weight"
            value={form.weight}
            onChange={(e) => setForm((f) => ({ ...f, weight: e.target.value }))}
            placeholder="Weight"
            type="number"
          />
          <button onClick={handleSave} disabled={loading}>
            {loading ? 'Saving...' : 'Save'}
          </button>
          <button onClick={() => setEdit(false)}>Cancel</button>
        </>
      ) : (
        <>
          <div>
            <b>Name:</b> {rider.fullName}
          </div>
          <div>
            <b>Racing Number:</b> {rider.racingNumber}
          </div>
          <div>
            <b>Country:</b> {rider.countryFlag} {rider.countryName}
          </div>
          <div>
            <b>Height:</b> {rider.height} cm
          </div>
          <div>
            <b>Weight:</b> {rider.weight} kg
          </div>
          <div>
            <b>Status:</b> {rider.isActive ? 'Active' : 'Retired'}
          </div>
          <div>
            <b>Team:</b> {rider.currentTeamName || 'Free Agent'}
            {rider.currentTeamId && <RemoveRiderButton riderId={id} onRemoved={() => window.location.reload()} />}
          </div>
          <button onClick={handleEdit}>Edit Info</button>
        </>
      )}
      <hr />
      <h3>Transfer to Team</h3>
      <TransferButton riderId={id} teams={teams} onTransferred={() => window.location.reload()} />
      <hr />
      <h3>Retire</h3>
      <input type="date" value={retireDate} onChange={(e) => setRetireDate(e.target.value)} />
      <button onClick={handleRetire} disabled={loading}>
        Retire
      </button>
      <button onClick={handleComeback} disabled={loading}>
        Comeback
      </button>
      <hr />
      <button
        onClick={() => {
          if (window.confirm('Bạn có chắc chắn muốn xóa rider này không?')) handleDelete();
        }}
        style={{ color: 'red' }}
        disabled={loading}
      >
        Delete Rider
      </button>
    </div>
  );
}
