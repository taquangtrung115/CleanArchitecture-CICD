import React from 'react';

const TeamCard = ({ team, onClick }) => (
    <div onClick={onClick} style={{ border: '1px solid #ccc', borderRadius: 8, padding: 16, minWidth: 220, cursor: 'pointer' }}>
        <div style={{ fontWeight: 'bold', fontSize: 18 }}>{team.name} ({team.shortName})</div>
        <div>{team.countryFlag} {team.countryName}</div>
        <div>Founded: {team.foundedYear?.slice(0, 4)}</div>
        <div>Status: {team.isActive ? 'Active' : 'Inactive'}</div>
        <div>Capacity: {team.capacity || 2}</div>
        <div>Description: {team.description}</div>
    </div>
);

export default TeamCard;
