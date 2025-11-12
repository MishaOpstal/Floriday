import React from "react";
import { Form } from "react-bootstrap";

interface NumberInputProps {
    label: string;
    name: string;
    placeholder?: string;
    value?: number | string;
    step?: number | "any";
    onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const NumberInput: React.FC<NumberInputProps> = ({ label, name, placeholder, value, step = 1, onChange }) => {
    return (
        <Form.Label>
            {label}
            <Form.Control
                type="number"
                name={name}
                placeholder={placeholder}
                step={step}
                value={value}
                onChange={onChange}
            />
        </Form.Label>
    );
};

export default NumberInput;
