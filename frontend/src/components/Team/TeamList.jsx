import React, { useEffect, useState } from 'react';
import { getTeams } from '../../api/teams';
import TeamCard from './TeamCard';

const TeamList = ({ token, filters = {}, onSelect }) => {
  const [teams, setTeams] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    setLoading(true);
    getTeams(filters, token)
      .then((res) => setTeams(res.data.items))
      .catch(setError)
      .finally(() => setLoading(false));
  }, [filters, token]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error loading teams.</div>;

  return (
    <div style={{ display: 'flex', flexWrap: 'wrap', gap: 16 }}>
      {teams.map((team) => (
        <TeamCard key={team.id} team={team} onClick={() => onSelect?.(team)} />
      ))}
    </div>
  );
};

export default TeamList;
