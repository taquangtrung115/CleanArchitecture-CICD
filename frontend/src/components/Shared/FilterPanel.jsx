import React from 'react';

const FilterPanel = ({ filters, onChange, options }) => (
  <div style={{ display: 'flex', gap: 16, marginBottom: 16 }}>
    {options.map((opt) => (
      <label key={opt.name}>
        {opt.label}:
        <select value={filters[opt.name] || ''} onChange={(e) => onChange(opt.name, e.target.value)}>
          <option value="">All</option>
          {opt.choices.map((c) => (
            <option key={c.value} value={c.value}>
              {c.label}
            </option>
          ))}
        </select>
      </label>
    ))}
  </div>
);

export default FilterPanel;
