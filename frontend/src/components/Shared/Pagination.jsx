import React from 'react';

const Pagination = ({ page, pageSize, total, onChange }) => {
  const totalPages = Math.ceil(total / pageSize);
  if (totalPages <= 1) return null;
  return (
    <div style={{ margin: '16px 0' }}>
      <button onClick={() => onChange(page - 1, pageSize)} disabled={page <= 1}>
        Prev
      </button>
      <span style={{ margin: '0 12px' }}>
        Page {page} of {totalPages}
      </span>
      <button onClick={() => onChange(page + 1, pageSize)} disabled={page >= totalPages}>
        Next
      </button>
    </div>
  );
};

export default Pagination;
