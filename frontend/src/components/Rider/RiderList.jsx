import React, { useEffect, useState } from 'react';
import { getRiders } from '../../api/riders';
import RiderCard from './RiderCard';

const RiderList = ({ token, filters = {}, onSelect }) => {
    const [riders, setRiders] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        setLoading(true);
        getRiders(filters, token)
            .then(res => setRiders(res.data.items))
            .catch(setError)
            .finally(() => setLoading(false));
    }, [filters, token]);

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error loading riders.</div>;

    return (
        <div style={{ display: 'flex', flexWrap: 'wrap', gap: 16 }}>
            {riders.map(rider => (
                <RiderCard key={rider.id} rider={rider} onClick={() => onSelect?.(rider)} />
            ))}
        </div>
    );
};

export default RiderList;
