import React from 'react';

const RiderCard = ({ rider, onClick }) => (
    <div onClick={onClick} style={{ border: '1px solid #ccc', borderRadius: 8, padding: 16, minWidth: 220, cursor: 'pointer' }}>
        <div style={{ fontWeight: 'bold', fontSize: 18 }}>{rider.fullName} #{rider.racingNumber}</div>
        <div>{rider.nickname && <span>"{rider.nickname}"</span>}</div>
        <div>{rider.countryFlag} {rider.countryName}</div>
        <div>Team: {rider.currentTeamName || 'Free Agent'}</div>
        <div>Status: {rider.isActive ? 'Active' : 'Retired'}</div>
        <div>Age: {rider.age}</div>
        <div>Height: {rider.height} cm | Weight: {rider.weight} kg</div>
    </div>
);

export default RiderCard;
