'use client';

import React from "react";
import { Button } from "react-bootstrap";
import { useRouter } from "next/navigation";

type ActionButtonsProps = {
    onDelete?: () => void;
    onUpdate?: () => void;
};

const ActionButtons: React.FC<ActionButtonsProps> = ({ onDelete, onUpdate }) => {
    const router = useRouter();

    return (
        <div className="d-flex gap-2">
            <Button
                variant="danger"
                size="sm"
                onClick={onDelete}
            >
                <i className="bi bi-trash"></i> Delete
            </Button>
            <Button
                variant="primary"
                size="sm"
                onClick={onUpdate}
            >
                <i className="bi bi-pencil"></i> Update
            </Button>
        </div>
    );
};

export default ActionButtons;
