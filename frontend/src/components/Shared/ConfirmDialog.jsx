import React from 'react';

const ConfirmDialog = ({ open, title, message, onConfirm, onCancel }) => {
    if (!open) return null;
    return (
        <div style={{ position: 'fixed', top: 0, left: 0, width: '100vw', height: '100vh', background: 'rgba(0,0,0,0.3)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000 }}>
            <div style={{ background: '#fff', padding: 32, borderRadius: 8, minWidth: 320 }}>
                <h3>{title}</h3>
                <div>{message}</div>
                <div style={{ marginTop: 24, display: 'flex', gap: 16, justifyContent: 'flex-end' }}>
                    <button onClick={onCancel}>Cancel</button>
                    <button onClick={onConfirm} style={{ background: '#d32f2f', color: '#fff' }}>Confirm</button>
                </div>
            </div>
        </div>
    );
};

export default ConfirmDialog;
