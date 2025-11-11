'use client';

import React from "react";
import { Button } from "react-bootstrap";
import { useRouter } from "next/navigation";
import styles from "./SmallButton.module.css";

type ActionButtonsProps = {
    onDelete?: () => void;
    onUpdate?: () => void;
};

const ActionButtons: React.FC<ActionButtonsProps> = ({ onDelete, onUpdate }) => {
    const router = useRouter();

    return (
        <div className="d-flex gap-2">
            <Button className={styles.transparentButton}
                variant="light"
                size="sm"
                onClick={onDelete}
            >
                <i
                    style={{ color: "var(--primary-background)" }}
                    className="bi bi-trash"
                ></i>
            </Button>
            <Button  className={styles.transparentButton}
                variant="light"
                size="sm"
                onClick={onUpdate}
            >
                <i
                    style={{ color: "var(--primary-background)" }}
                    className="bi bi-pencil"
                ></i>

            </Button>
        </div>
    );
};

export default ActionButtons;
