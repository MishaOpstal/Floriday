import React from "react";
import { Form, InputGroup } from "react-bootstrap";

interface NumberInputProps {
    label: string;
    name: string;
    placeholder?: string;
    value?: number | string;
    step?: number | "any";
    onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;

    // NEW:
    prefix?: string;
    postfix?: string;
}

const NumberInput: React.FC<NumberInputProps> = ({
                                                     label,
                                                     name,
                                                     placeholder,
                                                     value,
                                                     step = 1,
                                                     onChange,
                                                     prefix,
                                                     postfix
                                                 }) => {
    const control = (
        <Form.Control
            type="number"
            name={name}
            placeholder={placeholder}
            step={step}
            value={value}
            onChange={onChange}
        />
    );

    return (
        <Form.Group>
            <Form.Label>{label}</Form.Label>

            {prefix || postfix ? (
                <InputGroup className="mb-3">
                    {prefix && (
                        <InputGroup.Text id={`${name}-prefix`}>{prefix}</InputGroup.Text>
                    )}

                    {control}

                    {postfix && (
                        <InputGroup.Text id={`${name}-postfix`}>{postfix}</InputGroup.Text>
                    )}
                </InputGroup>
            ) : (
                control
            )}
        </Form.Group>
    );
};

export default NumberInput;
