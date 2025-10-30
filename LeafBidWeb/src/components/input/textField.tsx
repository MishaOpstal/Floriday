import s from './input.module.css';

interface TextFieldProps {
    placeholder?: string;
}

export default function TextField({ placeholder }: TextFieldProps) {
    return (
            <input type="text" className={s.input} placeholder={placeholder} />
    )
}