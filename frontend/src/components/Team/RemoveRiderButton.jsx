import React, { useState } from 'react';
import { transferRider } from '../../api/riders';
import { getErrorMessage } from '../../utils/errorHandler';

export default function RemoveRiderButton({ riderId, onRemoved }) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleRemove = async () => {
    if (!window.confirm('Bạn có chắc chắn muốn xóa rider này khỏi team không?')) return;
    setLoading(true);
    // Chuyển rider về free agent (teamId null)
    const res = await transferRider(riderId, { teamId: null, seasonId: '', joinDate: '' }, localStorage.getItem('accessToken'));
    setLoading(false);
    if (res.status === 200) {
      onRemoved?.();
    } else {
      setError(getErrorMessage(res) || 'Remove failed');
    }
  };

  return (
    <>
      <button onClick={handleRemove} disabled={loading} style={{ color: 'red', marginLeft: 8 }}>
        Remove
      </button>
      {error && <span style={{ color: 'red', marginLeft: 8 }}>{error}</span>}
    </>
  );
}
