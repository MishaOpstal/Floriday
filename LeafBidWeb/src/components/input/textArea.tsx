import s from './input.module.css';

interface TextAreaProps {
    placeholder?: string;
}

export default function TextArea({ placeholder }: TextAreaProps) {
    return (
            <textarea className={`${s.input} ${s.textArea}`} placeholder={placeholder} />
    )
}