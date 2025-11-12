import React from "react";
import { Form } from "react-bootstrap";

interface TextInputProps {
    label: string;
    name: string;
    placeholder?: string;
    value?: string;
    onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const TextInput: React.FC<TextInputProps> = ({ label, name, placeholder, value, onChange }) => {
    return (
        <Form.Label>
            {label}
            <Form.Control
                type="text"
                name={name}
                placeholder={placeholder}
                value={value}
                onChange={onChange}
            />
        </Form.Label>
    );
};

export default TextInput;
