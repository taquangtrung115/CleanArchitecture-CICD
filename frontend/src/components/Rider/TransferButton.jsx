import React, { useState } from 'react';
import { transferRider } from '../../api/riders';

export default function TransferButton({ riderId, teams, onTransferred }) {
    const [show, setShow] = useState(false);
    const [teamId, setTeamId] = useState('');
    const [seasonId, setSeasonId] = useState('');
    const [joinDate, setJoinDate] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const handleTransfer = async () => {
        setLoading(true);
        const res = await transferRider(riderId, { teamId, seasonId, joinDate }, localStorage.getItem('accessToken'));
        setLoading(false);
        if (res.status === 200) {
            setShow(false);
            onTransferred?.();
        } else {
            setError(res.error?.message || 'Transfer failed');
        }
    };

    return (
        <>
            <button onClick={() => setShow(s => !s)} style={{ marginLeft: 8 }}>Transfer</button>
            {show && (
                <div style={{ marginTop: 8, background: '#eee', padding: 8, borderRadius: 4 }}>
                    <select value={teamId} onChange={e => setTeamId(e.target.value)}>
                        <option value="">Select Team</option>
                        {teams.map(t => <option key={t.id} value={t.id}>{t.name}</option>)}
                    </select>
                    <input type="text" placeholder="Season ID" value={seasonId} onChange={e => setSeasonId(e.target.value)} />
                    <input type="date" value={joinDate} onChange={e => setJoinDate(e.target.value)} />
                    <button onClick={handleTransfer} disabled={loading}>OK</button>
                    <button onClick={() => setShow(false)}>Cancel</button>
                    {error && <span style={{ color: 'red', marginLeft: 8 }}>{error}</span>}
                </div>
            )}
        </>
    );
}
