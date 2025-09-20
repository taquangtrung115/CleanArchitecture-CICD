import React from 'react';

const TeamDetail = ({ team }) => {
  if (!team) return <div>Select a team to view details.</div>;
  return (
    <div style={{ border: '1px solid #aaa', borderRadius: 8, padding: 24, minWidth: 320 }}>
      <h2>
        {team.name} ({team.shortName})
      </h2>
      <div>
        Country: {team.countryFlag} {team.countryName}
      </div>
      <div>Founded: {team.foundedYear?.slice(0, 10)}</div>
      <div>Status: {team.isActive ? 'Active' : 'Inactive'}</div>
      <div>Capacity: {team.capacity || 2}</div>
      <div>Description: {team.description}</div>
    </div>
  );
};

export default TeamDetail;
