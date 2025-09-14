import React from 'react';

const RiderDetail = ({ rider }) => {
    if (!rider) return <div>Select a rider to view details.</div>;
    return (
        <div style={{ border: '1px solid #aaa', borderRadius: 8, padding: 24, minWidth: 320 }}>
            <h2>{rider.fullName} #{rider.racingNumber}</h2>
            <div>Nickname: {rider.nickname}</div>
            <div>Country: {rider.countryFlag} {rider.countryName}</div>
            <div>Date of Birth: {rider.dateOfBirth?.slice(0, 10)}</div>
            <div>Age: {rider.age}</div>
            <div>Height: {rider.height} cm</div>
            <div>Weight: {rider.weight} kg</div>
            <div>Status: {rider.isActive ? 'Active' : 'Retired'}</div>
            <div>Debut: {rider.debutDate?.slice(0, 10)}</div>
            <div>Retirement: {rider.retirementDate?.slice(0, 10) || 'N/A'}</div>
            <div>Years in MotoGP: {rider.yearsInMotoGP}</div>
            <div>Current Team: {rider.currentTeamName || 'Free Agent'}</div>
            <div>Created: {rider.createdDate?.slice(0, 10)}</div>
            <div>Modified: {rider.modifiedDate?.slice(0, 10) || 'N/A'}</div>
        </div>
    );
};

export default RiderDetail;
