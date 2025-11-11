import React, { useState, useEffect } from "react";
import { InputGroup, Form, Row, Col } from "react-bootstrap";
import s from "./DateSelect.module.css";

interface DateSelectProps {
    placeholder?: string;
    onSelect: (date: string | null) => void;
    delay?: number;
    defaultValue?: string; // optional ISO datetime for initial state
    useTime?: boolean;
}

/**
 * A reusable Bootstrap date (+time) selector with debounced change events.
 */
const DateSelect: React.FC<DateSelectProps> = ({
                                                   placeholder = "Select date...",
                                                   onSelect,
                                                   delay = 300,
                                                   defaultValue = "",
                                                   useTime = false,
                                               }) => {
    const [date, setDate] = useState<string>("");
    const [time, setTime] = useState<string>("");

    // Initialize if defaultValue (ISO string like "2025-11-11T14:30")
    useEffect(() => {
        if (defaultValue) {
            const [d, t] = defaultValue.split("T");
            setDate(d);
            if (useTime && t) setTime(t.slice(0, 5)); // keep HH:mm
        }
    }, [defaultValue, useTime]);

    // Debounce change callback
    useEffect(() => {
        const handler = setTimeout(() => {
            if (date) {
                const result = useTime && time ? `${date}T${time}` : date;
                onSelect(result);
            } else {
                onSelect(null);
            }
        }, delay);
        return () => clearTimeout(handler);
    }, [date, time, delay, onSelect, useTime]);

    return (
        <div className={`mb-3 ${s.inputGroup}`}>
            <Row className="g-2">
                <Col xs={useTime ? 7 : 12}>
                    <Form.Control
                        type="date"
                        value={date}
                        placeholder={placeholder}
                        onChange={(e) => setDate(e.target.value)}
                        className={s.formControl}
                    />
                </Col>
                {useTime && (
                    <Col xs={5}>
                        <Form.Control
                            type="time"
                            value={time}
                            onChange={(e) => setTime(e.target.value)}
                            className={s.formControl}
                        />
                    </Col>
                )}
            </Row>
        </div>
    );
};

export default DateSelect;
